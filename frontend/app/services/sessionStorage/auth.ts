export function setAccessToken(accessToken: string) {
  sessionStorage.setItem("accessToken", accessToken);
}

export function getAccessToken(): string | null {
  return sessionStorage.getItem("accessToken");
}

export function removeAccessToken() {
  sessionStorage.removeItem("accessToken");
}
