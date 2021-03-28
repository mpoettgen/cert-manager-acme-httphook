![Certificates for your private Kubernetes][banner]

[![Build and Test][build-badge]][build-link]
[![License][license-badge]][license-link]
[![Open Issues][issues-open-badge]][issues-open-link]
[![Closed Issues][issues-closed-badge]][issues-closed-link]
[![Open Pull Requests][pulls-open-badge]][pulls-open-link]
[![Closed Pull Requests][pulls-closed-badge]][pulls-closed-link]
[![Latest Release][release-badge]][release-link]
[![Docker Pulls][docker-pulls-badge]][docker-pulls-link]

# cert-manager-acme-httphook

cert-manager-acme-httphook provides a Kubernetes operator to hook into [cert-manager][cert-manager-homepage].

cert-manager automates management and issuance of TLS certificates from various issuing sources. Among those sources are certificate authorities (CAs) that implement the ACME protocol. Whenever cert-manager needs to order a new certificate from such an issuer, it creates a [ACME http-01][cert-manager-http01] challenge object with the challenge information provided by the CA. The default behavior of cert-manager is to present an http-01 challenge to the CA via an ingress. However, this does not work, when the the ingress is not reachable from the CA.

cert-manager-acme-httphook watches for challenge objects in Kubernetes and uploads challenges to a public SFTP server, so that they can be verified by the CA.

## Installation

> TODO

## Configuration

> TODO

## Quick Links

* [cert-manager][cert-manager-homepage]
> TODO

## Logo

Certificate by Adrien Coquet from the Noun Project

[banner]: https://github.com/mpoettgen/cert-manager-acme-httphook/blob/main/docs/assets/social.png?raw=true
[release-badge]: https://img.shields.io/github/v/tag/mpoettgen/cert-manager-acme-httphook?label=version&logo=github
[release-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/releases
[build-badge]: https://github.com/mpoettgen/cert-manager-acme-httphook/actions/workflows/ci.yml/badge.svg
[build-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/actions/workflows/ci.yml
[license-badge]: https://img.shields.io/github/license/mpoettgen/cert-manager-acme-httphook
[license-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/blob/main/LICENSE
[issues-open-badge]: https://img.shields.io/github/issues/mpoettgen/cert-manager-acme-httphook?logo=github
[issues-open-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/issues?q=is%3Aissue+is%3Aopen
[issues-closed-badge]: https://img.shields.io/github/issues-closed/mpoettgen/cert-manager-acme-httphook?logo=github
[issues-closed-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/issues?q=is%3Aissue+is%3Aclosed
[pulls-open-badge]: https://img.shields.io/github/issues-pr/mpoettgen/cert-manager-acme-httphook?logo=github
[pulls-open-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/pulls?q=is%3Apr+is%3Aopen
[pulls-closed-badge]: https://img.shields.io/github/issues-pr-closed/mpoettgen/cert-manager-acme-httphook?logo=github
[pulls-closed-link]: https://github.com/mpoettgen/cert-manager-acme-httphook/pulls?q=is%3Apr+is%3Aclosed
[docker-pulls-badge]: https://img.shields.io/docker/pulls/mpoettgen/cert-manager-acme-httphook
[docker-pulls-link]: https://hub.docker.com/r/mpoettgen/cert-manager-acme-httphook
[cert-manager-homepage]: https://certmanager.io
[cert-manager-http01]: https://cert-manager.io/docs/configuration/acme/http01/
