import { LogWorkDialogComponent } from './log-work-dialog/log-work-dialog.component';
import { HabitCategoryDto, HabitCategoryServiceProxy, HabitLogType, HabitServiceProxy } from "./../../shared/service-proxies/service-proxies";
import { CreateOrEditHabitDialogComponent } from "./create-or-edit-habit-dialog/create-or-edit-habit-dialog.component";
import { Component, HostListener, OnInit } from "@angular/core";
import { HabitDto } from "@shared/service-proxies/service-proxies";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from 'lodash';

@Component({
  selector: "app-habit-manager",
  templateUrl: "./habit-manager.component.html",
  styleUrls: ["./habit-manager.component.css"],
  animations: [appModuleAnimation()],
})
export class HabitManagerComponent implements OnInit {
  searchKeyWord : string ;
  categoryIdSelected: number = -1 ;
  categorySelected: HabitCategoryDto = null;
  
  habits : HabitDto[];
  listHabitCategory: HabitCategoryDto[];
  categoryPracticePercent: number = 0;

  constructor(
    private _modalService: BsModalService,
    private _habitService: HabitServiceProxy,
    private _habitCategoryService: HabitCategoryServiceProxy
  ) {}

  ngOnInit(): void {
     // Get all habit category
     this._habitCategoryService.getAll("",0,500).subscribe((res)=>{
      this.listHabitCategory = res.items;
      // if(this.listHabitCategory.length>0){
      //   this.categoryIdSelected = this.listHabitCategory[0].id;
      // }else{
      //   this.categoryIdSelected = -1;
      // }
      this.getAllHabits();
    });
  }

  // @HostListener('window:scroll', ['$event'])
  // onScroll(event) {
  //   var bodyEl = document.getElementsByTagName('body')[0];
  //   if (bodyEl.offsetHeight + bodyEl.scrollTop >= bodyEl.scrollHeight) {
  //     console.log("hahah");
  //   }
  // }

  getAllHabits() {
    this._habitService.getAllNoPaging(this.searchKeyWord,this.categoryIdSelected).subscribe((res)=>{
      this.habits = res; 
      if(this.categoryIdSelected!=-1){
        this.categorySelected['totalPracticeTime'] = _.sumBy(this.habits,(h)=>h.practiceTime); 
        this.categoryPracticePercent = Number((this.categorySelected['totalPracticeTime'] * 100/ this.categorySelected.goalTime).toFixed(4));
      }
    });
  }

  onHabitCategoryChange(){
    if(this.categoryIdSelected!=-1){
      this.categorySelected = _.find(this.listHabitCategory,(c)=>c.id==this.categoryIdSelected);
      this.categorySelected['totalPracticeTime'] = 0;
    }else{
      this.categorySelected = null;
    }
    this.getAllHabits();
  }

  resetSearching(){
    this.searchKeyWord = "";
    this.categoryIdSelected = -1;
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
    createOrEditHabitDialog.content.onUpdated.subscribe((id) => {
      this.updateHabitItem(id,"update");
    });
    createOrEditHabitDialog.content.onCreated.subscribe((id) => {
      this.updateHabitItem(id,"create");
    });
  }

  // Cập nhật lại thông tin habit vừa được thêm hoặc update 
  updateHabitItem(id,mode) : void{
    this._habitService.get(id).subscribe(
      (res) => {
        if(mode=='update'){
          var index = _.findIndex(this.habits,{id:id});
          this.habits.splice(index,1,res);
        }else{
          this.habits.push(res);
        }
      },
      (err) => {
        console.log(err);
      }
    );

  }

  trackerHabitList(index,item){
    return item;
  }
 
  onLogWorkSuccess(timeLog){
    this.categorySelected['totalPracticeTime'] += timeLog;
    this.categoryPracticePercent = Number((this.categorySelected['totalPracticeTime'] * 100/ this.categorySelected.goalTime).toFixed(4));
  }


}
