import { LogWorkDialogComponent } from './log-work-dialog/log-work-dialog.component';
import { HabitLogType, HabitServiceProxy } from "./../../shared/service-proxies/service-proxies";
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
  searchKeyWord : string;
  habits : HabitDto[];
  constructor(
    private _modalService: BsModalService,
    private _habitService: HabitServiceProxy
  ) {}

  ngOnInit(): void {
    this.getAllHabits();
  }

  getAllHabits() {
    this._habitService.getAllNoPaging(this.searchKeyWord).subscribe((res)=>{
      this.habits = res;
    })
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
