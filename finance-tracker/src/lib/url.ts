const URL =
  process.env.NODE_ENV === "production"
    ? "http://localhost:8065/"
    : "https://localhost:44311/";
export default URL;
