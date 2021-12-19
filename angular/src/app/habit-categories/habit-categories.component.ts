import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from '@shared/paged-listing-component-base';
import {
  HabitCategoryServiceProxy,
  HabitCategoryDto,
  HabitCategoryDtoPagedResultDto,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditHabitCategoryComponent } from './create-or-edit-habit-category/create-or-edit-habit-category.component';

class PagedHabitcategorysRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  templateUrl: './habit-categories.component.html',
  animations: [appModuleAnimation()]
})
export class HabitCategoriesComponent extends PagedListingComponentBase<HabitCategoryDto> {
  habitcategorys: HabitCategoryDto[] = [];
  keyword = '';
  isActive: boolean | null;
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _habitcategoryService: HabitCategoryServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedHabitcategorysRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = this.isActive;

    this._habitcategoryService
      .getAll(
        request.keyword,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: HabitCategoryDtoPagedResultDto) => {
        this.habitcategorys = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(habitcategory: HabitCategoryDto): void {
    abp.message.confirm(
      this.l('HabitCategoryDeleteWarningMessage', habitcategory.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._habitcategoryService
            .delete(habitcategory.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }

  createHabitCategory(): void {
    this.showCreateOrEditHabitcategoryDialog();
  }

  editHabitCategory(habitcategory: HabitCategoryDto): void {
    this.showCreateOrEditHabitcategoryDialog(habitcategory.id);
  }

  showCreateOrEditHabitcategoryDialog(id?: number): void {
    let createOrEditHabitcategoryDialog: BsModalRef;
    if (!id) {
      createOrEditHabitcategoryDialog = this._modalService.show(
        CreateOrEditHabitCategoryComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditHabitcategoryDialog = this._modalService.show(
        CreateOrEditHabitCategoryComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditHabitcategoryDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  clearFilters(): void {
    this.keyword = '';
    this.isActive = undefined;
    this.getDataPage(1);
  }
}
