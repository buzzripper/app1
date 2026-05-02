export const getCookie = (cookieName: string) => {
    const name = `${cookieName}=`;
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(';');

    for (const cookie of cookies) {
        let currentCookie = cookie;
        while (currentCookie.startsWith(' ')) {
            currentCookie = currentCookie.substring(1);
        }

        if (currentCookie.startsWith(name)) {
            return currentCookie.substring(name.length, currentCookie.length);
        }
    }

    return '';
};
