import BaseResponseDto from "@/models/http/BaseReponseDto";
import DataResponseDto from "@/models/http/DataResponseDto";
import PageResponseDto from "@/models/http/PageResponseDto";
import InvestmentDto, {
  InvestmentOverviewDto,
  CreateOrUpdateInvestmentDto,
  InvestmentSelectDto,
  InvestmentStatus,
} from "@/models/investment/InvestmentDtos";
import qs from "qs";
import ajax from "../lib/ajax";

const investmentService = {
  // get all for select
  async getAllForSelect(
    channelId: number,
    keyword: string | undefined,
  ): Promise<DataResponseDto<InvestmentSelectDto[]>> {
    const res = await ajax.get(
      "/api/services/app/investment/GetAllForSelect?channelId=" +
        channelId +
        "&keyword=" +
        keyword,
    );
    return res.data;
  },
  // Get all paging
  async getAllOverview(
    skipCount: number,
    resultCount: number,
    ids: number[],
    status: InvestmentStatus | null,
  ): Promise<PageResponseDto<InvestmentOverviewDto>> {
    var url = "/api/services/app/Investment/GetAllOverview";
    const response = await ajax.get(url, {
      params: {
        skipCount: skipCount,
        resultCount: resultCount,
        ids: ids,
        status: status,
      },
      paramsSerializer: function (params) {
        return qs.stringify(params, { arrayFormat: "repeat" });
      },
    });
    return response.data;
  },
  // create investment
  async createInvestment(
    input: CreateOrUpdateInvestmentDto,
  ): Promise<BaseResponseDto> {
    const res = await ajax.post("/api/services/app/investment/create", input);
    return res.data as BaseResponseDto;
  },
  // get by Id
  async getInvestemntById(id: number): Promise<DataResponseDto<InvestmentDto>> {
    const res = await ajax.get("/api/services/app/investment/get", {
      params: {
        id: id,
      },
    });
    return res.data;
  },
  // update
  async editInvestment(
    input: CreateOrUpdateInvestmentDto,
  ): Promise<BaseResponseDto> {
    const res = await ajax.put("/api/services/app/investment/update", input);
    return res.data;
  },
};

export default investmentService;
