interface Investment {
  id?: number;
  stockCode?: string;
  companyName?: string;
  totalBuy?: number;
  totalSell?: number;
  currentPrice?: number;
  vol?: number;
  description?: string;
}

export default Investment;
