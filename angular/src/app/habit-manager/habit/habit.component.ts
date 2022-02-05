import {
  HabitServiceProxy,
  HabitDto,
  HabitLogType,
  GetAllHabitLogOutputDto,
  HabitLogDto,
} from "./../../../shared/service-proxies/service-proxies";
import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild,
} from "@angular/core";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { LogWorkDialogComponent } from "../log-work-dialog/log-work-dialog.component";
import * as moment from "moment";
import { find as _find } from 'lodash-es';
import {
  ApexAxisChartSeries,
  ApexTitleSubtitle,
  ApexDataLabels,
  ApexChart,
  ApexPlotOptions,
  ChartComponent,
  ApexXAxis,
  ApexFill,
  ApexLegend,
  ApexTooltip,
  ApexYAxis,
} from "ng-apexcharts";

export type HeatMapChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
  plotOptions: ApexPlotOptions;
};

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
  selector: "app-habit",
  templateUrl: "./habit.component.html",
  styleUrls: ["./habit.component.css"],
})
export class HabitComponent implements OnInit {
  @ViewChild("chart") chart: ChartComponent;
  @Input("habit-dto") habitDto: HabitDto;
  @Output() onEditHabitBtnClick = new EventEmitter();
  @Output() onLogWorkSuccess = new EventEmitter();

  heatMapChartOptions: Partial<HeatMapChartOptions>;
  multipleYAxisChartOption: Partial<MultipleYAxisChartOptions>;
  progressValue: number;
  isShowDetail: boolean = false;
  listHabitLog : GetAllHabitLogOutputDto;
  rangeYears : number[] = [];
  chartYear: number = moment().year();
  heatMapChartData: GetAllHabitLogOutputDto[];
  timeDuration: number =7; // month

  constructor(
    private _habitService: HabitServiceProxy,
    private _modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.getRangeYear();
    this.progressValue = (this.habitDto.practiceTime * 100) / this.habitDto.timeGoal;
    this.setUpHeatMapChartOption();
    this.setMultipleYAxisChartOptions();
    this.showDetailChart();
  }

  getRangeYear(){
    let curentYear = moment().year();
    for(let i = 0;i<4;i++){
      this.rangeYears.push(curentYear-i);
    }
  }

  // Opend detail chart
  showDetailChart(){
    this.isShowDetail = !this.isShowDetail
    if(this.isShowDetail){
      this.getAllHabitLogDataByYear();
      this.getAllHabitLogInDuration();
    }
  }

  // Binding data for heatmap chart
  generateData(month) {
    var numberOfDays = moment(this.chartYear+"-"+month).daysInMonth();

    var i = 1;
    var series = [];
    var workLogInMonth = this.heatMapChartData ? this.heatMapChartData.find(ghl=>ghl.month==month) : null;

    while (i <= numberOfDays) {
      var x = i.toString();
      var y = 0;

      if(workLogInMonth && workLogInMonth.habitLogDtos){
        var loginDay =  workLogInMonth.habitLogDtos.find(hl=>hl.dateLog.date()==i);
        if(loginDay){
          y = loginDay.status;
        }
      }

      series.push({
        x: x,
        y: y,
      });
      i++;
    };
    return series;
  }

 

  // Sửa habit
  editHabit() {
    this.onEditHabitBtnClick.emit(this.habitDto.id);
  }

  // Lấy tất cả log của habit và binding vào heatmap chart
  getAllHabitLogDataByYear(){
    this._habitService.getAllHabitLogInYear(this.habitDto.id,this.chartYear).subscribe((res)=>{
      this.heatMapChartData = res;
      this.setUpHeatMapChartOption();
    });
  }

  getAllHabitLogInDuration(){
    this._habitService.getHabitLogByTime(this.habitDto.id,this.timeDuration).subscribe((res)=>{
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

  // Setup config for heatmap chart
  setUpHeatMapChartOption(){
    this.heatMapChartOptions = {
      series: [
        {
          name: "Jan",
          data: this.generateData(1),
        },
        {
          name: "Feb",
          data: this.generateData(2),
        },
        {
          name: "Mar",
          data: this.generateData(3),
        },
        {
          name: "Apr",
          data: this.generateData(4),
        },
        {
          name: "May",
          data: this.generateData(5),
        },
        {
          name: "Jun",
          data: this.generateData(6),
        },
        {
          name: "Jul",
          data: this.generateData(7),
        },
        {
          name: "Aug",
          data: this.generateData(8),
        },
        {
          name: "Sep",
          data: this.generateData(9),
        },
        {
          name: "Oct",
          data: this.generateData(10),
        },
        {
          name: "Nov",
          data: this.generateData(11),
        },
        {
          name: "Dec",
          data: this.generateData(12),
        },
      ],
      chart: {
        height: 350,
        type: "heatmap",
      },
      plotOptions: {
        heatmap: {
          shadeIntensity: 0.5,
          colorScale: {
            ranges: [
              {
                from: 0,
                to: 1,
                name: "Không hoàn thành",
                color: "#e4e4e4",
              },
              {
                from: 1,
                to: 2,
                name: "Hoàn thành",
                color: "#ffc132",
              }
            ],
          },
        },
      },
      dataLabels: {
        enabled: false,
      },
      title: {
        text: "HeatMap Chart",
      },
    };
  }

  // Setup bar chart option
  setMultipleYAxisChartOptions(){
    this.multipleYAxisChartOption ={
      series: [
        {
          name: "Working time (minute)",
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

  // Logwork dialog
  showLogWorkDialog(habitId: number, logType: HabitLogType): void {
    let logWorkDialog: BsModalRef = this._modalService.show(
      LogWorkDialogComponent,
      {
        initialState: {
          habitId: habitId,
          logType: logType,
        },
      }
    );
    logWorkDialog.content.onSave.subscribe((timeLog) => {
      this.getAllHabitLogDataByYear();
      this.getAllHabitLogInDuration();
      this.onLogWorkSuccess.emit(timeLog)
    });
  }

}
