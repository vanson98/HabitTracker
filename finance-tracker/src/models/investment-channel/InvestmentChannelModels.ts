export default interface InvestmentChannelDto{
  id: number;
  channelCode: string;
  changnelName: string;
  moneyInput: number;
  moneyOutput: number;
  moneyStock: number;
  buyFee: number;
  sellFee: number;
  totalBuyFee: number;
  totalSellFee: number;
}

export interface InvestmentChannelOverviewDto extends InvestmentChannelDto{
  nav: number;
  marketValueOfStocks: number;
  valueOfStocks: number;
}
export interface ChannelSellectDto {
  id: number;
  code: string;
  name: string;
}
