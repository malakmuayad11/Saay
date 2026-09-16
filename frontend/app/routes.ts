import { type RouteConfig, index, route } from "@react-router/dev/routes";

export default [
  // index("routes/home.tsx"),
  index("routes/signup.tsx"),
  route("signin", "routes/signin.tsx"),
] satisfies RouteConfig;
