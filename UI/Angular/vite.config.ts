import { defineConfig } from 'vite';

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    port: 4201,
    host: 'localhost',
    strictPort: true,
    hmr: {
      // Force HMR to connect directly to Vite server, not through BFF proxy
      host: 'localhost',
      port: 4201,
      protocol: 'wss',
      clientPort: 4201
    }
  },
  optimizeDeps: {
    force: true
  },
  build: {
    outDir: '../server/wwwroot',
    emptyOutDir: true
  }
});
