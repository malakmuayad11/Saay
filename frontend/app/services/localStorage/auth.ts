export function setAccessToken(accessToken: string) {
  localStorage.setItem("accessToken", accessToken);
}

export function setRefreshToken(refreshToken: string) {
  localStorage.setItem("refreshToken", refreshToken);
}

export function getAccessToken(): string | null {
  return localStorage.getItem("accessToken");
}
