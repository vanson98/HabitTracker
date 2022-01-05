interface BaseResponseDto {
  targetUrl: string;
  success: boolean;
  error: {
    message: string;
  };
  unAuthorizedRequest: boolean;
}
export default BaseResponseDto;
