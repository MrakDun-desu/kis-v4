type AuthEventType = "unauthorized";

const listeners = new Set<() => void>();

export const authEvents = {
  emit: (_evt: AuthEventType) => listeners.forEach((fn) => fn()),
  on: (_evt: AuthEventType, fn: () => void) => {
    listeners.add(fn);
    return () => listeners.delete(fn);
  }
}
