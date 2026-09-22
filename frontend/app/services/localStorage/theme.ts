export function setTheme(input: string) {
  localStorage.setItem("theme", input);
}

export function getTheme() {
  return localStorage.getItem("theme");
}
