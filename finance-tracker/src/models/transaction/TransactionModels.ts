export default interface TransactionDto {
  id?: number;
  price?: number;
  numberOfShares?: number;
  transactionTime?: Date;
  createDate?: Date;
  updateDate?: Date;
  transactionType?: TransactionType;
  investmentId?: number;
}

export interface SearchTransactionInputDto {
  investmentId?: number;
  fromTransactionDate?: Date;
  toTransactionDate?: Date;
  transactionType?: TransactionType;
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
