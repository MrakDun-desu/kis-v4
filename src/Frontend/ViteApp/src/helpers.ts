
export const capitalize = (input: string) => input[0].toUpperCase() + input.slice(1);

export const redirect = (url: string) => {
  window.location.href = `${import.meta.env.BASE_URL}${url}`;
}
