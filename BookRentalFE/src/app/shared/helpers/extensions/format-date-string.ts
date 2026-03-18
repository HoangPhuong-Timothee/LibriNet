export function formatDateString(dateString: string): string {
  let parts = dateString.split('/');
  let day = parts[0];
  let month = parts[1];
  let year = parts[2];
  return `${year}-${month}-${day}`
}
