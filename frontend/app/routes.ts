import { type RouteConfig, index, route } from "@react-router/dev/routes";

export default [
  // index("routes/home.tsx"),
  index("routes/signup.tsx"),
  route("signin", "routes/signin.tsx"),
  route("dashboard", "routes/dashboard.tsx", [
    route("goals", "routes/goals.tsx"),
  ]),
] satisfies RouteConfig;
