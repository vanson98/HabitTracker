import BaseResponseDto from "@/models/http/BaseReponseDto";
import DataResponseDto from "@/models/http/DataResponseDto";
import PageResponseDto from "@/models/http/PageResponseDto";
import Investment from "@/models/investment/Investment";
import InvestmentSelectDto from "@/models/investment/InvestmentSelectDto";
import ajax from "../lib/ajax";

const financeService = {
  // get all for select
  async getAllForSelect(): Promise<DataResponseDto<InvestmentSelectDto[]>> {
    const res = await ajax.get("/api/services/app/investment/GetAllForSelect");
    return res.data;
  },
  // Get all paging
  async getAllPaginInvestment(
    skipCount: number,
    resultCount: number,
  ): Promise<PageResponseDto<Investment>> {
    const response = await ajax.get("/api/services/app/Investment/GetAll", {
      params: {
        skipCount: skipCount,
        maxResultCount: resultCount,
      },
    });
    return response.data;
  },
  // create investment
  async createInvestment(input: Investment): Promise<BaseResponseDto> {
    const res = await ajax.post("/api/services/app/investment/create", input);
    return res.data as BaseResponseDto;
  },
  // get by Id
  async getInvestemntById(id: number): Promise<DataResponseDto<Investment>> {
    const res = await ajax.get("/api/services/app/investment/get", {
      params: {
        id: id,
      },
    });
    return res.data;
  },
  // update
  async editInvestment(input: Investment): Promise<BaseResponseDto> {
    const res = await ajax.put("/api/services/app/investment/update", input);
    return res.data;
  },
};

export default financeService;