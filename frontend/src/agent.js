import superagentPromise from 'superagent-promise';
import _superagent from 'superagent';

const superagent = superagentPromise(_superagent, global.Promise);

const API_ROOT =
  process.env.NODE_ENV === 'development'
    ? 'http://localhost:5000'
    : process.env.TasksBoard_Backend;

const responseBody = (res) => res.body;

let token = null;
const tokenPlugin = (req) => {
  if (token) {
    req.set('authorization', `Token ${token}`);
  }
};

var unauthorizedPlugin = (req) => {
  req.on('response', (res) => {
    if (res.status === 401) {
      //TODO: handle redirect to login
    }
  });
};

const requests = {
  del: (url) =>
    superagent
      .del(`${API_ROOT}${url}`)
      .use(tokenPlugin)
      .use(unauthorizedPlugin)
      .then(responseBody),
  get: (url) =>
    superagent
      .get(`${API_ROOT}${url}`)
      .use(tokenPlugin)
      .use(unauthorizedPlugin)
      .then(responseBody),
  put: (url, body) =>
    superagent
      .put(`${API_ROOT}${url}`, body)
      .use(tokenPlugin)
      .use(unauthorizedPlugin)
      .then(responseBody),
  post: (url, body) =>
    superagent
      .post(`${API_ROOT}${url}`, body)
      .use(tokenPlugin)
      .use(unauthorizedPlugin)
      .then(responseBody),
};

const Auth = {
  current: () => requests.get('/user'),
  login: (email, password) =>
    requests.post('/users/login', { user: { email, password } }),
  register: (name, email, password) =>
    requests.post('/users', { user: { name, email, password } }),
  save: (user) => requests.put('/user', { user }),
};

const Board = {
  create: (name) => requests.post('/board', { board: { name } }),
  all: () => requests.get('/board'),
  list: (name) => requests.get(`/board?name=${name}`),
  get: (id) => requests.get(`/board/${id}`),
  edit: (id, payload) =>
    requests.put(`/board/${id}`, { boardId: id, board: { ...payload } }),
  delete: (id) => requests.del(`/board/${id}`),
};

const Column = {
  get: (id) => requests.get(`/column/${id}`),
  create: (header, color, boardId) =>
    requests.post('/column', {
      column: { header: header, color: color, boardId: boardId },
    }),
  edit: (id, payload) =>
    requests.put(`/column/${id}`, {
      columnId: id,
      column: { ...payload },
    }),
  delete: (id) => requests.del(`/column/${id}`),
};

const Task = {
  create: (header, description = '', columnId) =>
    requests.post('/task', { task: { header, description, columnId } }),
  delete: (id) => {
    requests.del(`/task/${id}`);
  },
  edit: (id, payload) =>
    requests.put(`/task/${id}`, {
      taskId: id,
      task: { ...payload },
    }),
};

export default {
  Auth,
  Board,
  Column,
  Task,
  setToken: (_token) => {
    token = _token;
  },
};
