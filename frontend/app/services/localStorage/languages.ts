export function setI18nextLng(input: string) {
  localStorage.setItem("i18nextLng", input);
}

export function getI18nextLng() {
  return localStorage.getItem("i18nextLng");
}

export function setLanguage(input: string) {
  localStorage.setItem("language", input);
}

export function getLanguage() {
  return localStorage.getItem("language");
}
