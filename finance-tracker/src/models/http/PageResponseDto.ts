import BaseResponseDto from "./BaseReponseDto";
import PageResult from "./PageResult";

export default interface PageResponseDto<T> extends BaseResponseDto {
  result: PageResult<T>;
}
