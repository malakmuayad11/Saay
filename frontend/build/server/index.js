import { PassThrough } from "node:stream";
import { createReadableStreamFromReadable } from "@react-router/node";
import { Link, Links, Meta, Outlet, Scripts, ScrollRestoration, ServerRouter, UNSAFE_withComponentProps, UNSAFE_withErrorBoundaryProps, isRouteErrorResponse } from "react-router";
import { isbot } from "isbot";
import { renderToPipeableStream } from "react-dom/server";
import { jsx, jsxs } from "react/jsx-runtime";
import { clsx } from "clsx";
import { twMerge } from "tailwind-merge";
import { useState } from "react";
//#region \0rolldown/runtime.js
var __defProp = Object.defineProperty;
var __exportAll = (all, no_symbols) => {
	let target = {};
	for (var name in all) __defProp(target, name, {
		get: all[name],
		enumerable: true
	});
	if (!no_symbols) __defProp(target, Symbol.toStringTag, { value: "Module" });
	return target;
};
//#endregion
//#region node_modules/@react-router/dev/dist/config/defaults/entry.server.node.tsx
var entry_server_node_exports = /* @__PURE__ */ __exportAll({
	default: () => handleRequest,
	streamTimeout: () => streamTimeout
});
var streamTimeout = 5e3;
function handleRequest(request, responseStatusCode, responseHeaders, routerContext, loadContext) {
	if (request.method.toUpperCase() === "HEAD") return new Response(null, {
		status: responseStatusCode,
		headers: responseHeaders
	});
	return new Promise((resolve, reject) => {
		let shellRendered = false;
		let userAgent = request.headers.get("user-agent");
		let readyOption = userAgent && isbot(userAgent) || routerContext.isSpaMode ? "onAllReady" : "onShellReady";
		let timeoutId = setTimeout(() => abort(), 6e3);
		const { pipe, abort } = renderToPipeableStream(/* @__PURE__ */ jsx(ServerRouter, {
			context: routerContext,
			url: request.url
		}), {
			[readyOption]() {
				shellRendered = true;
				const body = new PassThrough({ final(callback) {
					clearTimeout(timeoutId);
					timeoutId = void 0;
					callback();
				} });
				const stream = createReadableStreamFromReadable(body);
				responseHeaders.set("Content-Type", "text/html");
				pipe(body);
				resolve(new Response(stream, {
					headers: responseHeaders,
					status: responseStatusCode
				}));
			},
			onShellError(error) {
				reject(error);
			},
			onError(error) {
				responseStatusCode = 500;
				if (shellRendered) console.error(error);
			}
		});
	});
}
//#endregion
//#region app/root.tsx
var root_exports = /* @__PURE__ */ __exportAll({
	ErrorBoundary: () => ErrorBoundary,
	Layout: () => Layout,
	default: () => root_default,
	links: () => links
});
var links = () => [
	{
		rel: "preconnect",
		href: "https://fonts.googleapis.com"
	},
	{
		rel: "preconnect",
		href: "https://fonts.gstatic.com",
		crossOrigin: "anonymous"
	},
	{
		rel: "stylesheet",
		href: "https://fonts.googleapis.com/css2?family=Inter:ital,opsz,wght@0,14..32,100..900;1,14..32,100..900&display=swap"
	}
];
function Layout({ children }) {
	return /* @__PURE__ */ jsxs("html", {
		lang: "en",
		children: [/* @__PURE__ */ jsxs("head", { children: [
			/* @__PURE__ */ jsx("meta", { charSet: "utf-8" }),
			/* @__PURE__ */ jsx("meta", {
				name: "viewport",
				content: "width=device-width, initial-scale=1"
			}),
			/* @__PURE__ */ jsx(Meta, {}),
			/* @__PURE__ */ jsx(Links, {})
		] }), /* @__PURE__ */ jsxs("body", { children: [
			children,
			/* @__PURE__ */ jsx(ScrollRestoration, {}),
			/* @__PURE__ */ jsx(Scripts, {})
		] })]
	});
}
var root_default = UNSAFE_withComponentProps(function App() {
	return /* @__PURE__ */ jsx(Outlet, {});
});
var ErrorBoundary = UNSAFE_withErrorBoundaryProps(function ErrorBoundary({ error }) {
	let message = "Oops!";
	let details = "An unexpected error occurred.";
	let stack;
	if (isRouteErrorResponse(error)) {
		message = error.status === 404 ? "404" : "Error";
		details = error.status === 404 ? "The requested page could not be found." : error.statusText || details;
	}
	return /* @__PURE__ */ jsxs("main", {
		className: "pt-16 p-4 container mx-auto",
		children: [
			/* @__PURE__ */ jsx("h1", { children: message }),
			/* @__PURE__ */ jsx("p", { children: details }),
			stack
		]
	});
});
//#endregion
//#region app/routes/home.tsx
var home_exports = /* @__PURE__ */ __exportAll({
	default: () => home_default,
	meta: () => meta$1
});
function meta$1({}) {
	return [{ title: "New React Router App" }, {
		name: "description",
		content: "Welcome to React Router!"
	}];
}
var home_default = UNSAFE_withComponentProps(function Home() {
	return /* @__PURE__ */ jsx("p", { children: "hi" });
});
//#endregion
//#region app/utils/index.ts
function cn(...inputs) {
	return twMerge(clsx(...inputs));
}
//#endregion
//#region app/components/form/Label.tsx
var Label = ({ htmlFor, children, className }) => {
	return /* @__PURE__ */ jsx("label", {
		htmlFor,
		className: cn("mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-400", className),
		children
	});
};
//#endregion
//#region app/components/form/input/InputField.tsx
var Input = ({ type = "text", id, name, placeholder, defaultValue, value, onChange, className = "", min, max, step, disabled = false, success = false, error = false, hint, ...props }) => {
	let inputClasses = ` h-11 w-full rounded-lg border appearance-none ps-4 pe-4 py-2.5 text-sm shadow-theme-xs placeholder:text-gray-400 focus:outline-hidden focus:ring-3  dark:bg-gray-900 dark:text-white/90 dark:placeholder:text-white/30 ${className}`;
	if (disabled) inputClasses += ` text-gray-500 border-gray-300 opacity-40 bg-gray-100 cursor-not-allowed dark:bg-gray-800 dark:text-gray-400 dark:border-gray-700 opacity-40`;
	else if (error) inputClasses += `  border-error-500 focus:border-error-300 focus:ring-error-500/20 dark:text-error-400 dark:border-error-500 dark:focus:border-error-800`;
	else if (success) inputClasses += `  border-success-500 focus:border-success-300 focus:ring-success-500/20 dark:text-success-400 dark:border-success-500 dark:focus:border-success-800`;
	else inputClasses += ` bg-transparent text-gray-800 border-gray-300 focus:border-brand-300 focus:ring-brand-500/20 dark:border-gray-700 dark:text-white/90  dark:focus:border-brand-800`;
	return /* @__PURE__ */ jsxs("div", {
		className: "relative",
		children: [/* @__PURE__ */ jsx("input", {
			type,
			id,
			name,
			placeholder,
			defaultValue,
			value,
			onChange,
			min,
			max,
			step,
			disabled,
			className: inputClasses,
			...props
		}), hint && /* @__PURE__ */ jsx("p", {
			className: `mt-1.5 text-xs ${error ? "text-error-500" : success ? "text-success-500" : "text-gray-500"}`,
			children: hint
		})]
	});
};
//#endregion
//#region app/validation.ts
var EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
var PASSWORD_REGEX = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z\d\s]).{8,}$/;
//#endregion
//#region app/components/auth/SignUpForm.tsx
function SignUpForm() {
	const [showPassword, setShowPassword] = useState(false);
	const [firstName, setFirstName] = useState("");
	const [firstNameValid, setFirstNameValid] = useState(true);
	const [lastName, setLastName] = useState("");
	const [lastNameValid, setLastNameValid] = useState(true);
	const [email, setEmail] = useState("");
	const [emailValid, setEmailValid] = useState(true);
	const [password, setPassword] = useState("");
	const [passwordValid, setPasswordValid] = useState(true);
	const [confirmPassword, setConfirmPassword] = useState("");
	const [confirmPasswordValid, setConfirmPasswordValid] = useState(true);
	const [error, setError] = useState(null);
	async function handleSignUp(e) {
		e.preventDefault();
		if (firstName === "" || lastName === "" || email === "" || password === "" || confirmPassword === "") return;
		if (!firstNameValid || !lastNameValid || !emailValid || !passwordValid || !confirmPasswordValid) return;
		const url = "https://saay.runasp.net/api/saay/users";
		const options = {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			body: JSON.stringify({
				firstName,
				lastName,
				email,
				password,
				profilePictureURL: null
			})
		};
		try {
			const response = await fetch(url, options);
			if (response.status === 400) {
				setError("Email already registered! Sign In instead.");
				return;
			}
			if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
			await response.json();
		} catch (error) {
			setError("An error occurred. Please try again later.");
		}
	}
	return /* @__PURE__ */ jsx("div", {
		className: "no-scrollbar flex w-full flex-1 flex-col overflow-y-auto lg:w-1/2",
		children: /* @__PURE__ */ jsxs("div", {
			className: "mx-auto flex w-full max-w-md flex-1 flex-col justify-center",
			children: [
				/* @__PURE__ */ jsx("div", { children: /* @__PURE__ */ jsxs("div", {
					className: "mb-5 sm:mb-8",
					children: [/* @__PURE__ */ jsx("h1", {
						className: "mt-4 mb-2 text-title-sm font-semibold text-gray-800 sm:text-title-md dark:text-white/90",
						children: "Sign Up"
					}), /* @__PURE__ */ jsx("p", {
						className: "text-sm text-gray-500 dark:text-gray-400",
						children: "Enter your email and password to sign up!"
					})]
				}) }),
				error && /* @__PURE__ */ jsx("div", {
					className: "my-2 bg-error-200 p-2 rounded-lg border-error-500",
					children: /* @__PURE__ */ jsx("p", {
						className: "text-error-500",
						children: error
					})
				}),
				/* @__PURE__ */ jsx("form", {
					onSubmit: handleSignUp,
					children: /* @__PURE__ */ jsxs("div", {
						className: "space-y-5",
						children: [
							/* @__PURE__ */ jsxs("div", {
								className: "grid grid-cols-1 gap-5 sm:grid-cols-2",
								children: [/* @__PURE__ */ jsxs("div", {
									className: "sm:col-span-1",
									children: [/* @__PURE__ */ jsxs(Label, {
										htmlFor: "fname",
										children: ["First Name", /* @__PURE__ */ jsx("span", {
											className: "text-error-500",
											children: "*"
										})]
									}), /* @__PURE__ */ jsx(Input, {
										type: "text",
										id: "fname",
										name: "fname",
										value: firstName,
										placeholder: "Enter your first name",
										hint: !firstNameValid ? "First Name is required" : void 0,
										error: !firstNameValid,
										onChange: (e) => setFirstName(e.target.value),
										onBlur: () => setFirstNameValid(firstName !== "")
									})]
								}), /* @__PURE__ */ jsxs("div", {
									className: "sm:col-span-1",
									children: [/* @__PURE__ */ jsxs(Label, {
										htmlFor: "lname",
										children: ["Last Name", /* @__PURE__ */ jsx("span", {
											className: "text-error-500",
											children: "*"
										})]
									}), /* @__PURE__ */ jsx(Input, {
										type: "text",
										id: "lname",
										name: "lname",
										value: lastName,
										placeholder: "Enter your last name",
										hint: !lastNameValid ? "Last Name is required" : void 0,
										error: !lastNameValid,
										onChange: (e) => setLastName(e.target.value),
										onBlur: () => setLastNameValid(lastName !== "")
									})]
								})]
							}),
							/* @__PURE__ */ jsxs("div", { children: [/* @__PURE__ */ jsxs(Label, {
								htmlFor: "email",
								children: ["Email", /* @__PURE__ */ jsx("span", {
									className: "text-error-500",
									children: "*"
								})]
							}), /* @__PURE__ */ jsx(Input, {
								type: "email",
								id: "email",
								name: "email",
								value: email,
								placeholder: "Enter your email",
								hint: !emailValid ? "Enter a valid email" : void 0,
								error: !emailValid,
								onChange: (e) => setEmail(e.target.value),
								onBlur: () => setEmailValid(EMAIL_REGEX.test(email))
							})] }),
							/* @__PURE__ */ jsxs("div", { children: [/* @__PURE__ */ jsxs(Label, {
								htmlFor: "password",
								children: ["Password", /* @__PURE__ */ jsx("span", {
									className: "text-error-500",
									children: "*"
								})]
							}), /* @__PURE__ */ jsxs("div", {
								className: "relative",
								children: [/* @__PURE__ */ jsx(Input, {
									id: "password",
									placeholder: "Enter your password",
									type: showPassword ? "text" : "password",
									value: password,
									hint: !passwordValid ? "Password must be at least 8 characters, contain a small, capital, special, and numeric characters." : void 0,
									error: !passwordValid,
									onChange: (e) => setPassword(e.target.value),
									onBlur: () => setPasswordValid(PASSWORD_REGEX.test(password))
								}), /* @__PURE__ */ jsx("span", {
									onClick: () => setShowPassword(!showPassword),
									className: "absolute inset-e-4 top-1/2 z-30 -translate-y-1/2 cursor-pointer"
								})]
							})] }),
							/* @__PURE__ */ jsxs("div", { children: [/* @__PURE__ */ jsxs(Label, {
								htmlFor: "confirmPassword",
								children: ["Confirm Password", /* @__PURE__ */ jsx("span", {
									className: "text-error-500",
									children: "*"
								})]
							}), /* @__PURE__ */ jsxs("div", {
								className: "relative",
								children: [/* @__PURE__ */ jsx(Input, {
									id: "confirmPassword",
									placeholder: "Confirm your password",
									type: showPassword ? "text" : "password",
									hint: !confirmPasswordValid ? "Passwords must match" : void 0,
									error: !confirmPasswordValid,
									onChange: (e) => setConfirmPassword(e.target.value),
									onBlur: () => setConfirmPasswordValid(confirmPassword !== "" && confirmPassword === password)
								}), /* @__PURE__ */ jsx("span", {
									onClick: () => setShowPassword(!showPassword),
									className: "absolute inset-e-4 top-1/2 z-30 -translate-y-1/2 cursor-pointer"
								})]
							})] }),
							/* @__PURE__ */ jsx("div", { children: /* @__PURE__ */ jsx("button", {
								type: "submit",
								className: "flex w-full items-center justify-center rounded-lg bg-brand-500 px-4 py-3 text-sm font-medium text-white shadow-theme-xs transition hover:bg-brand-600",
								children: "Sign Up"
							}) })
						]
					})
				}),
				/* @__PURE__ */ jsx("div", {
					className: "mt-5",
					children: /* @__PURE__ */ jsxs("p", {
						className: "text-center text-sm font-normal text-gray-700 sm:text-start dark:text-gray-400",
						children: [
							"Already have an account?",
							" ",
							/* @__PURE__ */ jsx(Link, {
								to: "/signin",
								className: "text-brand-500 hover:text-brand-600 dark:text-brand-400",
								children: "Sign In"
							})
						]
					})
				})
			]
		})
	});
}
//#endregion
//#region app/routes/signup.tsx
var signup_exports = /* @__PURE__ */ __exportAll({
	default: () => signup_default,
	meta: () => meta
});
var meta = () => [{ title: "Sign Up | Saay" }];
var signup_default = UNSAFE_withComponentProps(function SignUp() {
	return /* @__PURE__ */ jsx(SignUpForm, {});
});
//#endregion
//#region \0virtual:react-router/server-manifest
var server_manifest_default = {
	"entry": {
		"module": "/assets/entry.client-tULx6c2k.js",
		"imports": ["/assets/jsx-runtime-9DXhWUXR.js", "/assets/errorBoundaries-BAoyOSvd.js"],
		"css": []
	},
	"routes": {
		"root": {
			"id": "root",
			"parentId": void 0,
			"path": "",
			"index": void 0,
			"caseSensitive": void 0,
			"hasAction": false,
			"hasLoader": false,
			"hasClientAction": false,
			"hasClientLoader": false,
			"hasClientMiddleware": false,
			"hasDefaultExport": true,
			"hasErrorBoundary": true,
			"module": "/assets/root-QkMr9SeF.js",
			"imports": [
				"/assets/jsx-runtime-9DXhWUXR.js",
				"/assets/errorBoundaries-BAoyOSvd.js",
				"/assets/lib-Djhb_W_d.js"
			],
			"css": ["/assets/root-DGjb_wkw.css"],
			"clientActionModule": void 0,
			"clientLoaderModule": void 0,
			"clientMiddlewareModule": void 0,
			"hydrateFallbackModule": void 0
		},
		"routes/home": {
			"id": "routes/home",
			"parentId": "root",
			"path": void 0,
			"index": true,
			"caseSensitive": void 0,
			"hasAction": false,
			"hasLoader": false,
			"hasClientAction": false,
			"hasClientLoader": false,
			"hasClientMiddleware": false,
			"hasDefaultExport": true,
			"hasErrorBoundary": false,
			"module": "/assets/home-CL894NK2.js",
			"imports": ["/assets/jsx-runtime-9DXhWUXR.js"],
			"css": [],
			"clientActionModule": void 0,
			"clientLoaderModule": void 0,
			"clientMiddlewareModule": void 0,
			"hydrateFallbackModule": void 0
		},
		"routes/signup": {
			"id": "routes/signup",
			"parentId": "root",
			"path": "signup",
			"index": void 0,
			"caseSensitive": void 0,
			"hasAction": false,
			"hasLoader": false,
			"hasClientAction": false,
			"hasClientLoader": false,
			"hasClientMiddleware": false,
			"hasDefaultExport": true,
			"hasErrorBoundary": false,
			"module": "/assets/signup-DU2TNFOb.js",
			"imports": [
				"/assets/jsx-runtime-9DXhWUXR.js",
				"/assets/lib-Djhb_W_d.js",
				"/assets/errorBoundaries-BAoyOSvd.js"
			],
			"css": [],
			"clientActionModule": void 0,
			"clientLoaderModule": void 0,
			"clientMiddlewareModule": void 0,
			"hydrateFallbackModule": void 0
		}
	},
	"url": "/assets/manifest-70994a5b.js",
	"version": "70994a5b",
	"sri": void 0
};
//#endregion
//#region \0virtual:react-router/server-build
var assetsBuildDirectory = "build\\client";
var basename = "/";
var future = {
	"unstable_enableNodeReadableStream": false,
	"unstable_optimizeDeps": false
};
var ssr = true;
var isSpaMode = false;
var prerender = [];
var routeDiscovery = {
	"mode": "lazy",
	"manifestPath": "/__manifest"
};
var publicPath = "/";
var entry = { module: entry_server_node_exports };
var routes = {
	"root": {
		id: "root",
		parentId: void 0,
		path: "",
		index: void 0,
		caseSensitive: void 0,
		module: root_exports
	},
	"routes/home": {
		id: "routes/home",
		parentId: "root",
		path: void 0,
		index: true,
		caseSensitive: void 0,
		module: home_exports
	},
	"routes/signup": {
		id: "routes/signup",
		parentId: "root",
		path: "signup",
		index: void 0,
		caseSensitive: void 0,
		module: signup_exports
	}
};
var allowedActionOrigins = false;
//#endregion
export { allowedActionOrigins, server_manifest_default as assets, assetsBuildDirectory, basename, entry, future, isSpaMode, prerender, publicPath, routeDiscovery, routes, ssr };
