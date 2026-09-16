export function setCurrentUser(userId: number) {
  localStorage.setItem("userId", userId.toString());
}

export function removeCurrentUser() {
  localStorage.removeItem("userId");
}
