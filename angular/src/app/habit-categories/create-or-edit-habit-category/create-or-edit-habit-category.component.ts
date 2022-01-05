import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter,
} from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { AppComponentBase } from "@shared/app-component-base";
import {
  HabitCategoryDto,
  HabitCategoryServiceProxy,
} from "@shared/service-proxies/service-proxies";

@Component({
  selector: "app-create-or-edit-habit-category",
  templateUrl: "./create-or-edit-habit-category.component.html",
  styleUrls: ["./create-or-edit-habit-category.component.css"],
})
export class CreateOrEditHabitCategoryComponent
  extends AppComponentBase
  implements OnInit
{
  id: number;
  saving = false;
  habitcategory: HabitCategoryDto = new HabitCategoryDto();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _habitcategoryService: HabitCategoryServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if(this.id != null){
      this._habitcategoryService.get(this.id).subscribe((habitCategory)=>{
        this.habitcategory = habitCategory;
        this.habitcategory.goalTime = this.habitcategory.goalTime/60;
      })
    }else{
      this.habitcategory.goalTime = 10000;
    }
  }

  save(): void {
    this.saving = true;
    // Dữ liệu ở db là phút nên phải nhân với 60
    this.habitcategory.goalTime = this.habitcategory.goalTime * 60;
    if (!this.id) {
      this._habitcategoryService.create(this.habitcategory).subscribe(
        () => {
          this.notify.info(this.l("SavedSuccessfully"));
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      );
    }else{
      this._habitcategoryService.update(this.habitcategory).subscribe(
        () => {
          this.notify.info(this.l("SavedSuccessfully"));
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      );
    }
  }
}
