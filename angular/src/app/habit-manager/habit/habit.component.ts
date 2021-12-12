import { HabitServiceProxy, HabitDto, HabitLogType } from './../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { LogWorkDialogComponent } from '../log-work-dialog/log-work-dialog.component';

@Component({
  selector: 'app-habit',
  templateUrl: './habit.component.html',
  styleUrls: ['./habit.component.css']
})
export class HabitComponent implements OnInit {
  
  @Input('habit-dto') habitDto: HabitDto;
  @Output() onEditHabitBtnClick = new EventEmitter();

  constructor(private _habitService: HabitServiceProxy,private _modalService: BsModalService) { }

  ngOnInit(): void {

  }

  getHabitInfo(){
    this._habitService.get(this.habitDto.id).subscribe((res)=>{
      this.habitDto = res;
    },
    (err)=>{
      console.log(err);
    })
  }

  editHabit(){
    this.onEditHabitBtnClick.emit(this.habitDto.id);
  }

  showLogWorkDialog(habitId: number,logType: HabitLogType,timeGoal: number) : void{
    let logWorkDialog : BsModalRef = this._modalService.show(
      LogWorkDialogComponent,
      {
        initialState: {
          habitId: habitId,
          logType: logType,
          timeGoal: timeGoal
        }
      }
    );
    
  }
}
