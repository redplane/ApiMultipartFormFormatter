// Class which stores information of token which is for authorizing user into system.
export class AuthorizationToken{
  //#region Properties

  // Code which is used for accessing to system.
  public code: string;

  // Life type of token.
  public lifeTime: number;

  // When the token should be expired.
  public expire: number;

  //#endregion
}
