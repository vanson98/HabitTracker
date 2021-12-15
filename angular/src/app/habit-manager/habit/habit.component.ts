import {
  HabitServiceProxy,
  HabitDto,
  HabitLogType,
  GetAllHabitLogOutputDto,
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
  markers: any; //ApexMarkers;
  stroke: any; //ApexStroke;
  yaxis: ApexYAxis[];
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
  legend: ApexLegend;
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

  heatMapChartOptions: Partial<HeatMapChartOptions>;
  multipleYAxisChartOption: Partial<MultipleYAxisChartOptions>;
  progressValue: number;
  isShowDetail: boolean = false;
  listHabitLog : GetAllHabitLogOutputDto;
  rangeYears : number[] = [];
  chartYear: number = moment().year();
  heatMapChartData: GetAllHabitLogOutputDto[];

  constructor(
    private _habitService: HabitServiceProxy,
    private _modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.getRangeYear();
    this.progressValue = (this.habitDto.practiceTime * 100) / this.habitDto.timeGoal;
    this.setUpHeatMapChartOption();
    this.setMultipleYAxisChartOptions();
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

  // Lấy thông tin của habit
  getHabitInfo() {
    this._habitService.get(this.habitDto.id).subscribe(
      (res) => {
        this.habitDto = res;
      },
      (err) => {
        console.log(err);
      }
    );
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
    this.multipleYAxisChartOption = {
      series: [
        {
          name: "Income",
          type: "column",
          data: [1.4, 2, 2.5, 1.5, 2.5, 2.8, 3.8, 4.6]
        },
        {
          name: "Revenue",
          type: "line",
          data: [20, 29, 37, 36, 44, 45, 50, 58]
        }
      ],
      chart: {
        height: 350,
        type: "line",
        stacked: false
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        width: [1, 1, 4]
      },
      title: {
        text: "XYZ - Stock Analysis (2009 - 2016)",
        align: "left",
        offsetX: 110
      },
      xaxis: {
        categories: [2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016]
      },
      yaxis: [
        {
          axisTicks: {
            show: true
          },
          axisBorder: {
            show: true,
            color: "#008FFB"
          },
          labels: {
            style: {
              colors: "#008FFB"
            }
          },
          title: {
            text: "Income (thousand crores)",
            style: {
              color: "#008FFB"
            }
          },
          tooltip: {
            enabled: true
          }
        },
        {
          seriesName: "Revenue",
          opposite: true,
          axisTicks: {
            show: true
          },
          axisBorder: {
            show: true,
            color: "#FEB019"
          },
          labels: {
            style: {
              colors: "#FEB019"
            }
          },
          title: {
            text: "Revenue (thousand crores)",
            style: {
              color: "#FEB019"
            }
          }
        }
      ],
      tooltip: {
        fixed: {
          enabled: true,
          position: "topLeft", // topRight, topLeft, bottomRight, bottomLeft
          offsetY: 30,
          offsetX: 60
        }
      },
      legend: {
        horizontalAlign: "left",
        offsetX: 40
      }
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
  }

}
