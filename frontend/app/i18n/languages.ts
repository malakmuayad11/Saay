import { UsFlagIcon, SaFlagIcon } from "../assets/icons";
import type React from "react";

export const locales = ["en", "ar"] as const;
export type Locale = (typeof locales)[number];
export const defaultLocale: Locale = "en";

export interface Language {
  id: Locale;
  name: string;
  shortName: string;
  dir: "ltr" | "rtl";
  FlagIcon: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  badge?: string;
}

export const languages: Language[] = [
  {
    id: "en",
    name: "English",
    shortName: "English",
    dir: "ltr",
    FlagIcon: UsFlagIcon,
  },
  {
    id: "ar",
    name: "Arabic",
    shortName: "Arabic",
    dir: "rtl",
    FlagIcon: SaFlagIcon,
    badge: "RTL",
  },
];

export function getLanguage(locale: Locale): Language {
  return languages.find((l) => l.id === locale) || languages[0];
}

export function isRtl(locale: Locale): boolean {
  return getLanguage(locale).dir === "rtl";
}
