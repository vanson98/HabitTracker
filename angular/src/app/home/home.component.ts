import { HabitCategoryAnalysisDto, HabitCategoryDto, HabitLogStatus, HabitServiceProxy, LogWorkInputDto, TaskDto } from './../../shared/service-proxies/service-proxies';
import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { HabitCategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { ApexAxisChartSeries, ApexChart, ApexFill, ApexTitleSubtitle, ApexTooltip, ApexXAxis, ApexYAxis } from 'ng-apexcharts';


export type MultipleYAxisChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis | ApexYAxis[];
  title: ApexTitleSubtitle;
  labels: string[];
  stroke: any; // ApexStroke;
  dataLabels: any; // ApexDataLabels;
  fill: ApexFill;
  tooltip: ApexTooltip;
}

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
})
export class HomeComponent extends AppComponentBase implements OnInit {

  habitCategoryAnalyses : HabitCategoryAnalysisDto[];
  multipleYAxisChartOption: Partial<MultipleYAxisChartOptions>;
  dailyTaskDtos : TaskDto[];
  notDailyTaskDtos : TaskDto[];
  timeDuration: number =7; // month

  constructor(injector: Injector,
    private _habitCategoryService: HabitCategoryServiceProxy,
    private _habitService: HabitServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    // get all category analyses
    this._habitCategoryService.getAllCategoryAnalysis().subscribe((res)=>{
      this.habitCategoryAnalyses = res;
    });
    // get all daily task
    this._habitService.getAllTasks(true).subscribe((res)=>{
      this.dailyTaskDtos = res;
    });
    //get all not daily tasks
    this._habitService.getAllTasks(false).subscribe((res)=>{
      this.notDailyTaskDtos = res;
    });
    this.setMultipleYAxisChartOptions();
    this.getTotalWorkingOfEachDay();
  }

  logWork(task : TaskDto) {
    var workLog : LogWorkInputDto = new LogWorkInputDto({
      habitId : task.habitId,
      dateLog : moment(Date.now()),
      status :  HabitLogStatus._1,
      timeLog : task.timeLog,
    })
    this._habitService.logWork(workLog).subscribe((res)=>{
      if(res.statusCode == 200){
        this.notify.success(res.message);
        this.ngOnInit();
      }else{
        this.notify.error(res.message);
      }
    });
  }

  
  getTotalWorkingOfEachDay(){
    this._habitService.getTotalWorkingOfEachDay(this.timeDuration).subscribe((res)=>{
      this.multipleYAxisChartOption.series[0].data = res.map(hl=>{
        return parseFloat((hl.timeLog/60).toFixed(2));
      });
      this.multipleYAxisChartOption.series[1].data = res.map(hl=>{
        return parseFloat((hl.accumulationTime/60).toFixed(2));
      });
      this.multipleYAxisChartOption.labels =  res.map(hl=>{
        return hl.dateAgo;
      });;
    });
  }
  
  // Setup bar chart option
  setMultipleYAxisChartOptions(){
    this.multipleYAxisChartOption ={
      series: [
        {
          name: "Working time (hour)",
          type: "column",
          data: []
        },
        {
          name: "Accumulative Time (hour)",
          type: "line",
          data: []
        }
      ],
      chart: {
        height: 350,
        type: "line"
      },
      stroke: {
        width: [0, 1]
      },
      title: {
        text: "Working time"
      },
      dataLabels: {
        enabled: false,
        enabledOnSeries: [1]
      },
      labels: [
       
      ],
      xaxis: {
        type: "datetime"
      },
      yaxis: [
        {
          title: {
            text: "Working time"
          }
        },
        {
          opposite: true,
          title: {
            text: "Accumulative Time"
          }
        }
      ]
    };
  }
}
