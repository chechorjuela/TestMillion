interface Config {
  api: {
    baseURL: string;
    timeout: number;
  };
}

export const config: Config = {
  api: {
    baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5106/api',
    timeout: 10000, // 10 seconds
  },
};