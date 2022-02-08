import PageResponseDto from "@/models/http/PageResponseDto";
import {
  SearchTransactionInputDto,
  SearchTransactionOutputDto,
} from "../models/transaction/TransactionModels";
import ajax from "@/lib/ajax";
import BaseResponseDto from "@/models/http/BaseReponseDto";
import DataResponseDto from "@/models/http/DataResponseDto";
import Transaction from "@/models/transaction/TransactionModels";
import { AxiosResponse } from "axios";

const transactionService = {
  async searchPaging(
    input: SearchTransactionInputDto,
  ): Promise<PageResponseDto<SearchTransactionOutputDto>> {
    var res = await ajax.get("/api/services/app/transaction/searchPaging", {
      params: input,
    });
    return res.data;
  },
  async getById(id: number): Promise<DataResponseDto<Transaction>> {
    var res = await ajax.get("/api/services/app/transaction/get", {
      params: {
        id: id,
      },
    });
    return res.data;
  },
  async create(transaction: Transaction): Promise<BaseResponseDto> {
    var res = await ajax.post(
      "/api/services/app/transaction/create",
      transaction,
    );
    return res.data;
  },
  async update(transaction: Transaction): Promise<BaseResponseDto> {
    var res = await ajax.put(
      "/api/services/app/transaction/update",
      transaction,
    );
    return res.data;
  },
  async delete(id: number): Promise<BaseResponseDto> {
    var res = await ajax.delete(
      `/api/services/app/Transaction/Delete?Id=${id}`,
    );
    return res.data;
  },
};

export default transactionService;
