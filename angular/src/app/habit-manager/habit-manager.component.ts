import { LogWorkDialogComponent } from './log-work-dialog/log-work-dialog.component';
import { HabitCategoryDto, HabitCategoryServiceProxy, HabitLogType, HabitServiceProxy } from "./../../shared/service-proxies/service-proxies";
import { CreateOrEditHabitDialogComponent } from "./create-or-edit-habit-dialog/create-or-edit-habit-dialog.component";
import { Component, OnInit } from "@angular/core";
import { HabitDto } from "@shared/service-proxies/service-proxies";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";

@Component({
  selector: "app-habit-manager",
  templateUrl: "./habit-manager.component.html",
  styleUrls: ["./habit-manager.component.css"],
  animations: [appModuleAnimation()],
})
export class HabitManagerComponent implements OnInit {
  searchKeyWord : string ;
  categorySelected: number = -1 ;

  habits : HabitDto[];
  listHabitCategory: HabitCategoryDto[];

  constructor(
    private _modalService: BsModalService,
    private _habitService: HabitServiceProxy,
    private _habitCategoryService: HabitCategoryServiceProxy
  ) {}

  ngOnInit(): void {
    this.getAllHabits();
     // Get all habit category
     this._habitCategoryService.getAll("",0,500).subscribe((res)=>{
      this.listHabitCategory = res.items;
    });
  }

  getAllHabits() {
    this._habitService.getAllNoPaging(this.searchKeyWord,this.categorySelected).subscribe((res)=>{
      this.habits = res;
    });
    
  }

  resetSearching(){
    this.searchKeyWord = "";
    this.categorySelected = -1;
    this.getAllHabits();
  }
  createHabit(): void {
    this.showCreateOrEditDialog();
  }

  editHabit(habitId:number): void {
    this.showCreateOrEditDialog(habitId);
  }

  showCreateOrEditDialog(id?: number): void {
    let createOrEditHabitDialog: BsModalRef;
    if (!id) {
      createOrEditHabitDialog = this._modalService.show(
        CreateOrEditHabitDialogComponent,
        {
          class: "modal-lg",
        }
      );
    } else {
      createOrEditHabitDialog = this._modalService.show(
        CreateOrEditHabitDialogComponent,
        {
          class: "modal-lg",
          initialState: {
            habitId: id,
          }, 
        }
      );
    }
    createOrEditHabitDialog.content.onSave.subscribe(() => {
      this.getAllHabits()
    });
  }

 
}
