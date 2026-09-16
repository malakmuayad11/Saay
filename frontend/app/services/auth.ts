const Base_URL = "https://saay.runasp.net/api/saay/auth/";

export async function login(
  email: string,
  password: string,
): Promise<string | number> {
  const url = new URL("login", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    body: JSON.stringify({
      email,
      password,
    }),
  };

  try {
    const response = await fetch(url, options);

    if (response.status === 401 || response.status === 404) {
      return "Invalid Credentials.";
    }

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Login error:", error);
    return "An error occurred. Please try again later.";
  }
}
