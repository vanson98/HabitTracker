export default interface LoginModel {
  userNameOrEmailAddress: string | null;
  password: string | null;
  rememberClient: boolean;
}
