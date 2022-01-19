import DataResponseDto from "@/models/http/DataResponseDto";
import ajax from "@/lib/ajax";
import InvestmentChannelDto, {
  ChannelSellectDto,
  InvestmentChannelOverviewDto,
} from "@/models/investment-channel/InvestmentChannelModels";

const investmentChannelService = {
  getAllChannel: async function (): Promise<
    DataResponseDto<ChannelSellectDto[]>
  > {
    const res = await ajax.get(
      "/api/services/app/InvestmentChannel/GetAllChannel",
    );
    return res.data;
  },
  getChannelOverview: async function (
    id: number,
  ): Promise<InvestmentChannelOverviewDto> {
    const res = await ajax.get(
      "/api/services/app/InvestmentChannel/GetChannelOverview",
      {
        params: {
          id: id,
        },
      },
    );
    return res.data;
  },
};

export default investmentChannelService;
