import { type RouteConfig, index, route } from "@react-router/dev/routes";

// export default [
//   // index("routes/home.tsx"),
//   index("routes/signup.tsx"),
//   route("signin", "routes/signin.tsx"),
//   route("dashboard", "routes/dashboard.tsx", [
//     route("goals", "routes/goals.tsx"),
//     route("tasks", "routes/tasks.tsx"),
//     route("habits", "routes/habits.tsx"),
//   ]),
// ] satisfies RouteConfig;

export default [
  index("routes/signin.tsx"),
  route("signup", "routes/signup.tsx"),

  route("dashboard", "routes/dashboard.tsx", [
    index("routes/home.tsx"),
    route("goals", "routes/goals.tsx"),
    route("tasks", "routes/tasks.tsx"),
    route("habits", "routes/habits.tsx"),
  ]),
] satisfies RouteConfig;
