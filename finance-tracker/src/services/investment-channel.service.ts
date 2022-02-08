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
  ): Promise<DataResponseDto<InvestmentChannelOverviewDto>> {
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
  addMoneyInput: async function (
    channelCode: string,
    income: number,
  ): Promise<DataResponseDto<InvestmentChannelDto>> {
    const res = await ajax.post(
      "/api/services/app/InvestmentChannel/AddMoneyInput",
      null,
      {
        params: {
          channelCode: channelCode,
          income: income,
        },
      },
    );
    return res.data;
  },
  withdrawMoney: async function (
    channelCode: string,
    value: number,
  ): Promise<DataResponseDto<InvestmentChannelDto>> {
    const res = await ajax.post(
      "/api/services/app/InvestmentChannel/WithdrawMoney",
      null,
      {
        params: {
          channelCode: channelCode,
          value: value,
        },
      },
    );
    return res.data;
  },
  updateFee: async function (
    channelId: number,
    type: string,
    value: number,
  ): Promise<DataResponseDto<InvestmentChannelDto>> {
    const res = await ajax.put(
      "/api/services/app/InvestmentChannel/UpdateFee",
      null,
      {
        params: {
          channelId: channelId,
          type: type,
          value: value,
        },
      },
    );
    return res.data;
  },
};

export default investmentChannelService;
