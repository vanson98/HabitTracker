export default interface InvestmentDto {
  id?: number;
  stockCode?: string;
  companyName?: string;
  totalAmountBuy?: number;
  totalMoneyBuy: number;
  capitalCost: number;
  marketPrice: number;
  totalAmountSell: number;
  totalMoneySell: number;
  vol?: number;
  channelId: number;
  description?: string;
  status: InvestmentStatus;
}

export interface InvestmentOverviewDto extends InvestmentDto {
  currentInterest: number;
  sellInterest: number;
}

export interface CreateOrUpdateInvestmentDto {
  id?: number;
  stockCode?: string;
  companyName?: string;
  marketPrice?: number;
  description?: string;
  channelId?: number;
}

export interface GetAllOverviewInputDto {
  ids?: string;
  status: InvestmentStatus;
}

export enum InvestmentStatus {
  NotActive = 0,
  Active = 1,
  BuyOut = 2,
}

export interface InvestmentSelectDto {
  id: number;
  stockCode: string;
  companyName: string;
}
