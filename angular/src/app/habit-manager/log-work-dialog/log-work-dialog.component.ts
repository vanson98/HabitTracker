import {
  HabitLogStatus,
  HabitLogType,
  HabitServiceProxy,
} from "./../../../shared/service-proxies/service-proxies";
import { Component, OnInit } from "@angular/core";
import { LogWorkInputDto } from "@shared/service-proxies/service-proxies";
import { BsModalRef } from "ngx-bootstrap/modal";
import * as moment from "moment";
import { NotifyService } from "abp-ng2-module";

@Component({
  selector: "app-log-work-dialog",
  templateUrl: "./log-work-dialog.component.html",
  styleUrls: ["./log-work-dialog.component.css"],
})
export class LogWorkDialogComponent implements OnInit {
  saving = false;
  habitId: number;
  logType: HabitLogType;
  workLog: LogWorkInputDto;
  workLogStatus: boolean = true;
  dateLogData: Date = new Date();

  constructor(public bsModalRef: BsModalRef,private _habitService: HabitServiceProxy, public notify: NotifyService) {}

  ngOnInit(): void {
    this.workLog = new LogWorkInputDto();
    this.workLog.habitId = this.habitId;
  }

  logWork() {
    this.saving = true;
    this.workLog.status = this.workLogStatus
      ? HabitLogStatus._1
      : HabitLogStatus._0;
    this.workLog.dateLog = moment(this.dateLogData);
    this._habitService.logWork(this.workLog).subscribe((res)=>{
      if(res.statusCode == 200){
        this.notify.success(res.message);
      }else{
        this.notify.error(res.message);
      }
      this.saving  = false;
      this.bsModalRef.hide();
    });
  }
}
