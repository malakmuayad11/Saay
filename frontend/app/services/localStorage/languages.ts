export function seti18nextLng(input: string) {
  localStorage.setItem("i18nextLng", input);
}

export function geti18nextLn() {
  return localStorage.getItem("i18nextLn");
}

export function setLanguage(input: string) {
  localStorage.setItem("language", input);
}

export function getLanguage() {
  localStorage.getItem("language");
}
