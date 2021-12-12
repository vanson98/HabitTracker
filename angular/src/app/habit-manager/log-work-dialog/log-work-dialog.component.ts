import { HabitLogType } from './../../../shared/service-proxies/service-proxies';
import { Component, OnInit } from "@angular/core";
import { LogWorkInputDto } from "@shared/service-proxies/service-proxies";
import { BsModalRef } from "ngx-bootstrap/modal";

@Component({
  selector: "app-log-work-dialog",
  templateUrl: "./log-work-dialog.component.html",
  styleUrls: ["./log-work-dialog.component.css"],
})
export class LogWorkDialogComponent implements OnInit {
  saving = false;
  habitId : number;
  logType : HabitLogType;
  timeGoal: number;
  workLog : LogWorkInputDto;

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit(): void {
    this.workLog = new LogWorkInputDto();
    this.workLog.habitId = this.habitId;
  }

  logWork(){
    console.log(this.workLog);
  }
}
