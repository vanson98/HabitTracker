import { HabitServiceProxy } from './../../../shared/service-proxies/service-proxies';

import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {
  HabitDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';

@Component({
  selector: 'app-create-or-edit-habit-dialog',
  templateUrl: './create-or-edit-habit-dialog.component.html',
  styleUrls: ['./create-or-edit-habit-dialog.component.css']
})
export class CreateOrEditHabitDialogComponent implements OnInit {

  habitId : number;
  habitDto: HabitDto = new HabitDto();
  saving = false;
  @Output() onSave = new EventEmitter<any>();

  constructor(public bsModalRef: BsModalRef,
    public _habitService: HabitServiceProxy,
    public notify: NotifyService) { }

  ngOnInit(): void {
    if(this.habitId != null){
      this._habitService.get(this.habitId).subscribe((res)=>{
        this.habitDto = res;
      })
    }
  }

  save(){
    this.saving = true;
    if(this.habitId){
      this._habitService.update(this.habitDto).subscribe(
        () => {
          this.notify.info("Cập nhật thành công");
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      )
    }else{
      this._habitService.create(this.habitDto).subscribe(
        () => {
          this.notify.info("Tạo mới thành công");
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
