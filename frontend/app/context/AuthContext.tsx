import {
  createContext,
  type Dispatch,
  type SetStateAction,
  useState,
  type ReactNode,
  useEffect,
  useContext,
} from "react";

import { getCurrentUser } from "~/services/localStorage/users";

type AuthContextType = {
  currentUserId: number | null;
  setCurrentUserId: Dispatch<SetStateAction<number | null>>;
  refreshUserData: () => void;
};

export const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [currentUserId, setCurrentUserId] = useState<number | null>(null);

  useEffect(() => {
    const userId = getCurrentUser();
    if (!userId) return;

    const id = parseInt(userId);
    if (Number.isNaN(id)) return;

    async function setUserProvider() {
      try {
        setCurrentUserId(id);
      } catch {
        console.error("Error while fetching user from IndexedDB");
      }
    }
    setUserProvider();
  }, []);

  const [userDataVersion, setUserDataVersion] = useState(0);

  const refreshUserData = () => {
    setUserDataVersion((prev) => prev + 1);
  };
  return (
    <AuthContext.Provider
      value={{
        currentUserId: currentUserId,
        setCurrentUserId: setCurrentUserId,
        refreshUserData: refreshUserData,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
