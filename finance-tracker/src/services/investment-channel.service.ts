import DataResponseDto from "@/models/http/DataResponseDto";
import ajax from "@/lib/ajax";
import { ChannelSellectDto } from "@/models/investment-channel/InvestmentChannelModels";

const investmentChannelService = {
  getAllChannel: async function (): Promise<
    DataResponseDto<ChannelSellectDto[]>
  > {
    const res = await ajax.get(
      "/api/services/app/InvestmentChannel/GetAllChannel",
    );
    return res.data;
  },
};

export default investmentChannelService;
