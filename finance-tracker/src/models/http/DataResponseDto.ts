import BaseResponseDto from "./BaseReponseDto";

export default interface DataResponseDto<T> extends BaseResponseDto {
  result: T;
}
