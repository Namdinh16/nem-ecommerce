import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'NemEcommerce',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44379/',
    redirectUri: baseUrl,
    clientId: 'NemEcommerce_App',
    responseType: 'code',
    scope: 'offline_access NemEcommerce',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44351',
      rootNamespace: 'NemEcommerce',
    },
  },
} as Environment;
