export function setRefreshToken(refreshToken: string) {
  localStorage.setItem("refreshToken", refreshToken);
}

export function removeRefreshToken() {
  localStorage.removeItem("refreshToken");
}
