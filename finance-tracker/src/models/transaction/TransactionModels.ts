import moment from "moment";

export default interface TransactionDto {
  id?: number;
  price?: number;
  numberOfShares?: number;
  transactionTime: Date;
  creationTime?: Date;
  lastModificationTime?: Date;
  transactionType?: TransactionType;
  investmentId?: number;
  totalFee: number;
}

export interface SearchTransactionInputDto {
  investmentIds: number[];
  fromTransactionDate?: string;
  toTransactionDate?: string;
  transactionType: number;
  skipCount: number;
  maxResultCount: number;
}

export interface SearchTransactionOutputDto extends TransactionDto {
  stockCode: string;
}

export enum TransactionType {
  BUY,
  SELL,
}
