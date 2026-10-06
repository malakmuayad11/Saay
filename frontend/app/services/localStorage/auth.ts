export function setRefreshToken(refreshToken: string) {
  localStorage.setItem("refreshToken", refreshToken);
}

export function removeRefreshToken() {
  localStorage.removeItem("refreshToken");
}

export function getRefreshToken() {
  return localStorage.getItem("refreshToken");
}
