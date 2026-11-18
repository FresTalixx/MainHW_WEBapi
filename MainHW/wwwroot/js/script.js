const tokenKey = 'token';
let editingCategoryId = null;
let allCategories = [];
let currentPage = 1;
let pageSize = 5;

const loginForm = document.getElementById('loginForm');
const registerForm = document.getElementById('registerForm');
const loginMessage = document.getElementById('loginMessage');
const logoutButton = document.getElementById('logoutButton');
const backgroundUploadPanel = document.getElementById('backgroundUploadPanel');
const backgroundForm = document.getElementById('backgroundForm');
const backgroundFile = document.getElementById('backgroundFile');
const backgroundMessage = document.getElementById('backgroundMessage');
const newCategoryFile = document.getElementById('newCategoryFile');
const newCategoryPreview = document.getElementById('newCategoryPreview');
const categoryFile = document.getElementById('categoryFile');
let newCategoryPreviewUrl = null;
const authStatus = document.getElementById('authStatus');
const categoryRows = document.getElementById('categoryRows');
const categoryMessage = document.getElementById('categoryMessage');
const addCategoryForm = document.getElementById('addCategoryForm');
const newCategoryName = document.getElementById('newCategoryName');
const editor = document.getElementById('editor');
const categoryForm = document.getElementById('categoryForm');
const categoryName = document.getElementById('categoryName');
const saveButton = document.getElementById('saveButton');
const categoryFilter = document.getElementById('categoryFilter');
const categoryOrder = document.getElementById('categoryOrder');
const categoryDirection = document.getElementById('categoryDirection');

function showAuthForm(mode) {
    const isLogin = mode === 'login';
    loginForm.hidden = !isLogin;
    registerForm.hidden = isLogin;
    document.getElementById('loginTab').classList.toggle('active', isLogin);
    document.getElementById('registerTab').classList.toggle('active', !isLogin);
    document.getElementById('authTitle').textContent = isLogin ? 'Sign in' : 'Create account';
    setMessage(loginMessage, '');
}

function setMessage(element, message, isError = false) {
    element.textContent = message;
    element.className = `message ${message ? (isError ? 'error' : 'success') : ''}`;
}

async function register(event) {
    event.preventDefault();
    const formData = new FormData(registerForm);
    setMessage(loginMessage, 'Creating account…');
    try {
        const result = await apiRequest('/api/user/register', {
            method: 'POST',
            body: JSON.stringify({
                name: formData.get('name'),
                email: formData.get('email'),
                password: formData.get('password')
            })
        });
        if (!result?.token) throw new Error('The registration response did not contain a token.');
        localStorage.setItem(tokenKey, result.token);
        updateAuthState();
        await loadUserBackgroundImage();
        registerForm.reset();
        setMessage(loginMessage, 'Account created. You are now signed in.');
        await loadCategories();
    } catch (error) {
        setMessage(loginMessage, error.message, true);
    }
}

function getToken() {
    return localStorage.getItem(tokenKey);
}

function updateAuthState() {
    const isAuthenticated = Boolean(getToken());
    authStatus.textContent = isAuthenticated ? 'Connected' : 'Not connected';
    authStatus.classList.toggle('connected', isAuthenticated);
    logoutButton.hidden = !isAuthenticated;
    backgroundUploadPanel.hidden = !isAuthenticated;
}

function setUserBackgroundImage(path) {
    const imagePath = typeof path === 'string' ? path.trim() : '';

    if (!imagePath) {
        document.body.style.backgroundImage = '';
        return;
    }

    const imageUrl = imagePath.startsWith('/') ? imagePath : `/uploads/${imagePath}`;
    document.body.style.backgroundImage = `url("${imageUrl.replace(/"/g, '%22')}")`;
}

async function loadUserBackgroundImage() {
    if (!getToken()) {
        setUserBackgroundImage(null);
        return;
    }

    try {
        const user = await apiRequest('/api/user/me');
        setUserBackgroundImage(user?.bgFileName);
    } catch {
        setUserBackgroundImage(null);
    }
}

async function uploadBackgroundImage(event) {
    event.preventDefault();
    const file = backgroundFile.files[0];
    if (!file || !requireAuthentication()) return;

    const formData = new FormData();
    formData.append('file', file);
    setMessage(backgroundMessage, 'Uploading…');

    try {
        await apiRequest('/api/UploadFile', { method: 'PUT', body: formData });
        backgroundForm.reset();
        await loadUserBackgroundImage();
        setMessage(backgroundMessage, 'Background image uploaded.');
    } catch (error) {
        setMessage(backgroundMessage, error.message, true);
    }
}

async function readResponse(response) {
    const text = await response.text();
    let body = null;
    try {
        body = text ? JSON.parse(text) : null;
    } catch {
        body = text;
    }

    if (!response.ok) {
        const validationErrors = body?.errors
            ? Object.values(body.errors).flat().join(' ')
            : null;
        throw new Error(validationErrors || body?.title || body?.message || body || `Request failed (${response.status})`);
    }

    return body;
}

async function apiRequest(path, options = {}) {
    const headers = { ...(options.headers || {}) };
    const token = getToken();
    if (token) headers['X-My-Authorization'] = token;
    if (options.body && !(options.body instanceof FormData)) headers['Content-Type'] = 'application/json';

    const response = await fetch(path, { ...options, headers });
    if (response.status === 401) {
        localStorage.removeItem(tokenKey);
        updateAuthState();
    }
    return readResponse(response);
}

function requireAuthentication() {
    if (getToken()) return true;
    setMessage(categoryMessage, 'Sign in before managing categories.', true);
    return false;
}

function formatDate(value) {
    if (!value) return '—';
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? value : date.toLocaleString();
}

function getVisibleCategories() {
    const filter = categoryFilter.value.trim().toLocaleLowerCase();
    const filtered = allCategories.filter(category => !filter || category.name.toLocaleLowerCase().includes(filter));
    const field = categoryOrder.value;
    const direction = categoryDirection.value === 'desc' ? -1 : 1;

    return filtered.sort((left, right) => {
        let leftValue = left[field];
        let rightValue = right[field];
        if (field === 'creationDate' || field === 'updateDate') {
            leftValue = leftValue ? new Date(leftValue).getTime() : 0;
            rightValue = rightValue ? new Date(rightValue).getTime() : 0;
        } else if (field === 'name') {
            leftValue = leftValue.toLocaleLowerCase();
            rightValue = rightValue.toLocaleLowerCase();
        }
        if (leftValue < rightValue) return -1 * direction;
        if (leftValue > rightValue) return 1 * direction;
        return 0;
    });
}

function renderCategoryView(resetPage = true) {
    if (resetPage) currentPage = 1;
    renderCategories(getVisibleCategories());
}

function getCategoryImageUrl(path) {
    if (typeof path !== 'string' || !path.trim()) return '';
    return path.startsWith('/') ? path : `/uploads/${path}`;
}

function renderCategories(categories) {
    categoryRows.innerHTML = '';
    if (!categories?.length) {
        currentPage = 1;
        categoryRows.innerHTML = '<tr><td colspan="6" class="empty">No categories found. Add the first one above.</td></tr>';
        updatePagination(0);
        return;
    }

    const pageCount = Math.ceil(categories.length / pageSize);
    currentPage = Math.min(currentPage, pageCount);
    const pageCategories = categories.slice((currentPage - 1) * pageSize, currentPage * pageSize);

    pageCategories.forEach(category => {
        const row = document.createElement('tr');
        const fileInputId = `category-file-${category.id}`;
        row.innerHTML = `
            <td class="category-image-cell">
                <img class="category-thumbnail" alt="">
                <input id="${fileInputId}" class="category-upload-input" type="file" accept="image/*">
                <label class="button category-upload-button" for="${fileInputId}">Upload image</label>
            </td>
            <td>${category.id}</td>
            <td><strong></strong></td>
            <td>${formatDate(category.creationDate)}</td>
            <td>${formatDate(category.updateDate)}</td>
            <td><div class="actions"><button class="secondary edit-button" type="button">Edit</button><button class="danger delete-button" type="button">Delete</button></div></td>`;
        row.querySelector('strong').textContent = category.name;
        const imageUrl = getCategoryImageUrl(category.bgFileName);
        const thumbnail = row.querySelector('.category-thumbnail');
        if (imageUrl) {
            thumbnail.src = imageUrl;
            thumbnail.alt = `${category.name} image`;
            const fileLink = document.createElement('a');
            fileLink.href = imageUrl;
            fileLink.textContent = 'Open file';
            fileLink.target = '_blank';
            fileLink.rel = 'noopener';
            row.querySelector('.category-image-cell').appendChild(fileLink);
        } else {
            thumbnail.hidden = true;
        }
        const fileInput = row.querySelector('.category-upload-input');
        fileInput.addEventListener('change', () => uploadCategoryImage(category, fileInput));
        row.querySelector('.edit-button').addEventListener('click', () => startEditing(category));
        row.querySelector('.delete-button').addEventListener('click', () => deleteCategory(category.id, category.name));
        categoryRows.appendChild(row);
    });
    updatePagination(pageCount);
}

async function uploadCategoryImage(category, fileInput) {
    const file = fileInput.files[0];
    if (!file || !requireAuthentication()) return;

    const formData = new FormData();
    formData.append('name', category.name);
    formData.append('file', file);
    setMessage(categoryMessage, `Uploading image for ${category.name}…`);

    try {
        await apiRequest(`/api/categories/${category.id}`, { method: 'PUT', body: formData });
        setMessage(categoryMessage, `Image uploaded for ${category.name}.`);
        await loadCategories();
    } catch (error) {
        setMessage(categoryMessage, error.message, true);
    } finally {
        fileInput.value = '';
    }
}

function updatePagination(pageCount) {
    const totalPages = pageCount || 1;
    document.getElementById('pageInfo').textContent = `Page ${currentPage} of ${totalPages}`;
    document.getElementById('previousPageButton').disabled = currentPage <= 1;
    document.getElementById('nextPageButton').disabled = currentPage >= totalPages;
}

async function loadCategories() {
    if (!requireAuthentication()) return;
    categoryRows.innerHTML = '<tr><td colspan="6" class="loading">Loading categories…</td></tr>';
    setMessage(categoryMessage, '');
    try {
        allCategories = await apiRequest('/api/categories');
        renderCategoryView();
    } catch (error) {
        categoryRows.innerHTML = '<tr><td colspan="6" class="empty">Unable to load categories.</td></tr>';
        setMessage(categoryMessage, error.message, true);
    }
}

async function signIn(event) {
    event.preventDefault();
    const formData = new FormData(loginForm);
    setMessage(loginMessage, 'Signing in…');
    try {
        const result = await apiRequest('/api/user/login', {
            method: 'POST',
            body: JSON.stringify({ email: formData.get('email'), password: formData.get('password') })
        });
        if (!result?.token) throw new Error('The login response did not contain a token.');
        localStorage.setItem(tokenKey, result.token);
        updateAuthState();
        await loadUserBackgroundImage();
        setMessage(loginMessage, 'Signed in successfully.');
        await loadCategories();
    } catch (error) {
        setMessage(loginMessage, error.message, true);
    }
}

async function addCategory(event) {
    event.preventDefault();
    if (!requireAuthentication()) return;
    try {
        const formData = new FormData();
        formData.append('name', newCategoryName.value.trim());
        if (newCategoryFile.files[0]) formData.append('file', newCategoryFile.files[0]);
        await apiRequest('/api/categories', { method: 'POST', body: formData });
        addCategoryForm.reset();
        updateNewCategoryPreview();
        setMessage(categoryMessage, 'Category added.');
        await loadCategories();
    } catch (error) {
        setMessage(categoryMessage, error.message, true);
    }
}

function updateNewCategoryPreview() {
    if (newCategoryPreviewUrl) {
        URL.revokeObjectURL(newCategoryPreviewUrl);
        newCategoryPreviewUrl = null;
    }

    const file = newCategoryFile.files[0];
    if (!file || !file.type.startsWith('image/')) {
        newCategoryPreview.removeAttribute('src');
        newCategoryPreview.hidden = true;
        return;
    }

    newCategoryPreviewUrl = URL.createObjectURL(file);
    newCategoryPreview.src = newCategoryPreviewUrl;
    newCategoryPreview.hidden = false;
}

function startEditing(category) {
    editingCategoryId = category.id;
    categoryName.value = category.name;
    categoryFile.value = '';
    saveButton.textContent = 'Save changes';
    editor.classList.add('visible');
    categoryName.focus();
}

function stopEditing() {
    editingCategoryId = null;
    categoryName.value = '';
    categoryFile.value = '';
    saveButton.textContent = 'Add category';
    editor.classList.remove('visible');
}

async function saveCategory(event) {
    event.preventDefault();
    if (!requireAuthentication() || editingCategoryId === null) return;
    try {
        const formData = new FormData();
        formData.append('name', categoryName.value.trim());
        if (categoryFile.files[0]) formData.append('file', categoryFile.files[0]);
        await apiRequest(`/api/categories/${editingCategoryId}`, { method: 'PUT', body: formData });
        stopEditing();
        setMessage(categoryMessage, 'Category updated.');
        await loadCategories();
    } catch (error) {
        setMessage(categoryMessage, error.message, true);
    }
}

async function deleteCategory(id, name) {
    if (!requireAuthentication() || !window.confirm(`Delete “${name}”?`)) return;
    try {
        await apiRequest(`/api/categories/${id}`, { method: 'DELETE' });
        setMessage(categoryMessage, 'Category deleted.');
        await loadCategories();
    } catch (error) {
        setMessage(categoryMessage, error.message, true);
    }
}

loginForm.addEventListener('submit', signIn);
registerForm.addEventListener('submit', register);
backgroundForm.addEventListener('submit', uploadBackgroundImage);
document.getElementById('loginTab').addEventListener('click', () => showAuthForm('login'));
document.getElementById('registerTab').addEventListener('click', () => showAuthForm('register'));
logoutButton.addEventListener('click', () => {
    localStorage.removeItem(tokenKey);
    setUserBackgroundImage(null);
    stopEditing();
    updateAuthState();
    categoryRows.innerHTML = '<tr><td colspan="6" class="loading">Sign in to load categories.</td></tr>';
    setMessage(loginMessage, 'Signed out.');
});
addCategoryForm.addEventListener('submit', addCategory);
newCategoryFile.addEventListener('change', updateNewCategoryPreview);
categoryForm.addEventListener('submit', saveCategory);
document.getElementById('cancelButton').addEventListener('click', stopEditing);
document.getElementById('refreshButton').addEventListener('click', loadCategories);
document.getElementById('previousPageButton').addEventListener('click', () => {
    if (currentPage > 1) {
        currentPage--;
        renderCategories(getVisibleCategories());
    }
});
document.getElementById('nextPageButton').addEventListener('click', () => {
    if (currentPage < Math.ceil(allCategories.length / pageSize)) {
        currentPage++;
        renderCategories(getVisibleCategories());
    }
});
document.getElementById('pageSize').addEventListener('change', event => {
    pageSize = Number(event.target.value);
    currentPage = 1;
    renderCategoryView(false);
});
categoryFilter.addEventListener('input', () => renderCategoryView());
categoryOrder.addEventListener('change', () => renderCategoryView());
categoryDirection.addEventListener('change', () => renderCategoryView(false));
document.getElementById('clearFiltersButton').addEventListener('click', () => {
    categoryFilter.value = '';
    categoryOrder.value = 'id';
    categoryDirection.value = 'asc';
    renderCategoryView();
});

updateAuthState();
if (getToken()) {
    loadUserBackgroundImage();
    loadCategories();
}
